window.QuestionWrapperComponent = {
  props: { question: Object },
  template: `
    <div class="question-wrapper">
      <label class="form-label">
        {{ question.title }}
        <span v-if="question.isRequired" class="text-danger">*</span>
      </label>
      <slot></slot>
      <div class="form-text" v-if="question.helpText">{{ question.helpText }}</div>
    </div>
  `
};
