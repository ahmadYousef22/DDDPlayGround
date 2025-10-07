(function(){
  const { createApp } = Vue;

  async function fetchJson(url){
    const res = await fetch(url, { cache: 'no-store' });
    if(!res.ok) throw new Error('Failed to load '+url);
    return await res.json();
  }

  const app = createApp({
    data(){
      return {
        loaded: false,
        pages: [],
        answersByQuestionId: {},
        validationErrorsByQuestionId: {}
      };
    },
    methods: {
      componentType(question){
        switch (question.questionTypeId){
          case 1: return 'true-false';
          case 2: return 'free-text';
          case 3: return 'multiple-choice';
          default: return 'free-text';
        }
      },
      updateAnswer(questionId, value){
        this.answersByQuestionId = { ...this.answersByQuestionId, [questionId]: value };
      },
      async load(url){
        const data = await fetchJson(url);
        this.pages = data.pages || [];
        this.answersByQuestionId = {};
        this.validationErrorsByQuestionId = {};
        this.loaded = true;
      },
      downloadAnswers(){
        const payload = {
          timestampUtc: new Date().toISOString(),
          answers: this.answersByQuestionId
        };
        const blob = new Blob([JSON.stringify(payload, null, 2)], { type: 'application/json' });
        const a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = 'survey-answers.json';
        a.click();
        URL.revokeObjectURL(a.href);
      },
      reset(){
        this.pages = [];
        this.answersByQuestionId = {};
        this.loaded = false;
      }
    }
  });

  app.component('question-wrapper', window.QuestionWrapperComponent);
  app.component('true-false', window.TrueFalseComponent);
  app.component('free-text', window.FreeTextComponent);
  app.component('multiple-choice', window.MultipleChoiceComponent);

  const vm = app.mount('#surveyApp');
  window.surveyApp = vm;
  window.loadSurvey = (url)=> vm.load(url);
})();
