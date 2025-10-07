(function(){
  window.FreeTextQuestion = {
    name: 'FreeTextQuestion',
    props: {
      modelValue: { type: String, default: '' },
      question: { type: Object, required: true }
    },
    emits: ['update:modelValue'],
    template: `
      <div>
        <textarea class="form-control" :id="'q_' + question.id" rows="3" :placeholder="question.placeholder || 'Type your answer...'" v-model="internalValue"></textarea>
      </div>
    `,
    data(){
      return { internalValue: this.modelValue };
    },
    watch: {
      internalValue(newVal){ this.$emit('update:modelValue', newVal); },
      modelValue(newVal){ if(newVal !== this.internalValue) this.internalValue = newVal; }
    }
  };
})();
