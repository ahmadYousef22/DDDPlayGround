window.MultipleChoiceComponent = {
  props: { question: Object, modelValue: [String, Array, null] },
  emits: ['update:modelValue'],
  computed: {
    isMulti(){ return !!this.question.allowMultiple; }
  },
  methods:{
    onChange(evt, option){
      if(!this.isMulti){
        this.$emit('update:modelValue', option.value);
        return;
      }
      const set = new Set(Array.isArray(this.modelValue) ? this.modelValue : []);
      if(evt.target.checked) set.add(option.value); else set.delete(option.value);
      this.$emit('update:modelValue', Array.from(set));
    }
  },
  template: `
    <question-wrapper :question="question">
      <div v-if="!isMulti">
        <div class="form-check" v-for="opt in question.options" :key="opt.value">
          <input class="form-check-input" type="radio"
                 :name="'q_'+question.id" :id="'q_'+question.id+'_'+opt.value"
                 :checked="modelValue===opt.value"
                 @change="onChange($event, opt)">
          <label class="form-check-label" :for="'q_'+question.id+'_'+opt.value">{{ opt.label }}</label>
        </div>
      </div>
      <div v-else>
        <div class="form-check" v-for="opt in question.options" :key="opt.value">
          <input class="form-check-input" type="checkbox"
                 :id="'q_'+question.id+'_'+opt.value"
                 :checked="Array.isArray(modelValue) && modelValue.includes(opt.value)"
                 @change="onChange($event, opt)">
          <label class="form-check-label" :for="'q_'+question.id+'_'+opt.value">{{ opt.label }}</label>
        </div>
      </div>
    </question-wrapper>
  `
};
