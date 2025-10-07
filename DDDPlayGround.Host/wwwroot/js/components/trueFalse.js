(function(){
  window.TrueFalseQuestion = {
    name: 'TrueFalseQuestion',
    props: {
      modelValue: { type: [Boolean, null], default: null },
      question: { type: Object, required: true }
    },
    emits: ['update:modelValue'],
    methods: {
      update(value){ this.$emit('update:modelValue', value); }
    },
    template: `
      <div>
        <div class="form-check">
          <input class="form-check-input" type="radio" :name="'q_' + question.id" :id="'q_' + question.id + '_yes'" value="true" :checked="modelValue===true" @change="update(true)" />
          <label class="form-check-label" :for="'q_' + question.id + '_yes'">Yes</label>
        </div>
        <div class="form-check">
          <input class="form-check-input" type="radio" :name="'q_' + question.id" :id="'q_' + question.id + '_no'" value="false" :checked="modelValue===false" @change="update(false)" />
          <label class="form-check-label" :for="'q_' + question.id + '_no'">No</label>
        </div>
      </div>
    `
  };
})();
