(function(){
  const { h } = Vue;
  window.QuestionWrapper = {
    name: 'QuestionWrapper',
    props: {
      question: { type: Object, required: true }
    },
    render() {
      const isMandatory = this.question?.isMandatory === true;
      return h('div', { class: 'question mb-3' }, [
        h('div', { class: 'question-header d-flex align-items-start mb-2' }, [
          h('span', { class: 'fw-semibold me-2' }, this.question?.questionTextEn || ''),
          isMandatory ? h('span', { class: 'mandatory text-danger' }, '*') : null
        ]),
        h('div', { class: 'question-body' }, this.$slots.default ? this.$slots.default() : [])
      ]);
    }
  };
})();
