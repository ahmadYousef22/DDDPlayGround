(function(){
  window.MultipleChoiceQuestion = {
    name: 'MultipleChoiceQuestion',
    props: {
      modelValue: { type: [String, Number, null], default: null },
      question: { type: Object, required: true }
    },
    emits: ['update:modelValue'],
    methods: {
      update(value){ this.$emit('update:modelValue', value); }
    },
    template: `
      <div>
        <div class="form-check" v-for="c in (question.choices || [])" :key="c.id">
          <input class="form-check-input" type="radio" :name="'q_' + question.id" :id="'q_' + question.id + '_' + c.id" :value="c.id" :checked="modelValue===c.id" @change="update(c.id)" />
          <label class="form-check-label" :for="'q_' + question.id + '_' + c.id">{{ c.text }}</label>
        </div>
      </div>
    `
  };
})();
