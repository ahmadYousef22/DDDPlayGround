window.FreeTextComponent = {
  props: { question: Object, modelValue: [String, null] },
  emits: ['update:modelValue'],
  template: `
    <question-wrapper :question="question">
      <input type="text" class="form-control"
             :placeholder="question.placeholder || ''"
             :value="modelValue || ''"
             @input="$emit('update:modelValue', $event.target.value)" />
    </question-wrapper>
  `
};
