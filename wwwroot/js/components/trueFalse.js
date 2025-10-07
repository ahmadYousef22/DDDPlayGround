window.TrueFalseComponent = {
  props: { question: Object, modelValue: [Boolean, null] },
  emits: ['update:modelValue'],
  template: `
    <question-wrapper :question="question">
      <div class="btn-group" role="group">
        <input type="radio" class="btn-check"
               :name="'q_'+question.id" :id="'q_yes_'+question.id"
               :checked="modelValue===true"
               @change="$emit('update:modelValue', true)">
        <label class="btn btn-outline-primary" :for="'q_yes_'+question.id">Yes</label>

        <input type="radio" class="btn-check"
               :name="'q_'+question.id" :id="'q_no_'+question.id"
               :checked="modelValue===false"
               @change="$emit('update:modelValue', false)">
        <label class="btn btn-outline-primary" :for="'q_no_'+question.id">No</label>
      </div>
    </question-wrapper>
  `
};
