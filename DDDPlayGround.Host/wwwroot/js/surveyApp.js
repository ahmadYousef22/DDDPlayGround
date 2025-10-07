(function(){
  const { createApp, reactive, computed } = Vue;

  const QuestionWrapper = window.QuestionWrapper;
  const TrueFalseQuestion = window.TrueFalseQuestion;
  const FreeTextQuestion = window.FreeTextQuestion;
  const MultipleChoiceQuestion = window.MultipleChoiceQuestion;

  const QuestionRenderer = {
    name: 'QuestionRenderer',
    components: { QuestionWrapper, TrueFalseQuestion, FreeTextQuestion, MultipleChoiceQuestion },
    props: { question: { type: Object, required: true }, modelValue: { type: [String, Number, Boolean, Object, null], default: null } },
    emits: ['update:modelValue'],
    computed: {
      typeId(){ return this.question?.questionTypeId; }
    },
    methods: {
      update(value){ this.$emit('update:modelValue', value); }
    },
    template: `
      <QuestionWrapper :question="question">
        <template v-if="typeId===1">
          <TrueFalseQuestion :question="question" :model-value="modelValue" @update:modelValue="update" />
        </template>
        <template v-else-if="typeId===2">
          <FreeTextQuestion :question="question" :model-value="modelValue" @update:modelValue="update" />
        </template>
        <template v-else-if="typeId===3">
          <MultipleChoiceQuestion :question="question" :model-value="modelValue" @update:modelValue="update" />
        </template>
        <template v-else>
          <div class="text-muted">Unsupported question type ({{ typeId }}).</div>
        </template>
      </QuestionWrapper>
    `
  };

  const SurveyPage = {
    name: 'SurveyPage',
    components: { QuestionRenderer },
    props: { page: { type: Object, required: true }, answers: { type: Object, required: true } },
    methods: {
      bindKey(q){ return `q_${q.id}`; },
      updateAnswer(q, value){ this.$emit('update:answers', { ...this.answers, [this.bindKey(q)]: value }); }
    },
    template: `
      <div class="page-block mb-4">
        <div class="mb-3"><h5 class="mb-0">Page {{ page.pageNumber }}</h5></div>
        <div>
          <QuestionRenderer v-for="q in (page.questions || [])" :key="q.id" :question="q" :model-value="answers[bindKey(q)]" @update:modelValue="val => updateAnswer(q, val)" />
        </div>
      </div>
    `
  };

  const App = {
    name: 'SurveyApp',
    components: { SurveyPage },
    setup(){
      const survey = reactive(window.surveyData || { name: '', pages: [] });
      const state = reactive({
        currentPageIndex: 0,
        answers: {},
        touched: {},
      });
      const totalPages = computed(() => (survey.pages || []).length);
      const currentPage = computed(() => (survey.pages || [])[state.currentPageIndex] || null);

      function nextPage(){ if(state.currentPageIndex < totalPages.value - 1){ state.currentPageIndex += 1; } }
      function prevPage(){ if(state.currentPageIndex > 0){ state.currentPageIndex -= 1; } }
      function onUpdateAnswers(newAnswers){ state.answers = newAnswers; }

      function validatePage(page){
        if(!page) return true;
        const questions = page.questions || [];
        for(const q of questions){
          if(q.isMandatory){
            const key = `q_${q.id}`;
            const val = state.answers[key];
            if(val === undefined || val === null || val === "") return false;
          }
        }
        return true;
      }

      function canGoNext(){ return validatePage(currentPage.value); }

      function submit(){
        if(!validatePage(currentPage.value)) return;
        console.log('Collected answers:', JSON.parse(JSON.stringify(state.answers)));
        alert('Thank you! Answers logged to console.');
      }

      return { survey, state, totalPages, currentPage, nextPage, prevPage, onUpdateAnswers, canGoNext, submit };
    },
    template: `
      <div>
        <div class="mb-3">
          <h4 class="mb-0">{{ survey.name }}</h4>
        </div>

        <SurveyPage v-if="currentPage" :page="currentPage" :answers="state.answers" @update:answers="onUpdateAnswers" />

        <div class="d-flex justify-content-between">
          <button class="btn btn-outline-secondary" :disabled="state.currentPageIndex===0" @click="prevPage">Previous</button>
          <div class="d-flex gap-2">
            <button class="btn btn-primary" v-if="state.currentPageIndex < totalPages - 1" :disabled="!canGoNext()" @click="nextPage">Next</button>
            <button class="btn btn-success" v-else :disabled="!canGoNext()" @click="submit">Submit</button>
          </div>
        </div>
      </div>
    `
  };

  createApp(App).mount('#surveyApp');
})();
