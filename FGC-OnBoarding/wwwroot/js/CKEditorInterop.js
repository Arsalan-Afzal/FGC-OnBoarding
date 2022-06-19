window.CKEditorInterop = (() => {
    const editors = {};

    return {
        init(id, dotNetReference) {
            ClassicEditor
				.create(document.getElementById(id), {

					toolbar: {
						items: [
							'heading',
							'|',
							'bold',
							'italic',
							'link',
							'bulletedList',
							'numberedList',
							'|',
							'indent',
							'outdent',
							'|',
							'imageUpload',
							'blockQuote',
							'insertTable',
							'undo',
							'redo'
						]
					},
					language: 'en',
					image: {
						toolbar: [
							'imageTextAlternative',
							'imageStyle:full',
							'imageStyle:side'
						]
					},
					table: {
						contentToolbar: [
							'tableColumn',
							'tableRow',
							'mergeTableCells'
						]
					},
					licenseKey: '',

				})
          
                .then(editor => {
                    editors[id] = editor;
                    editor.model.document.on('change:data', () => {
                        let data = editor.getData();

                        const el = document.createElement('div');
                        el.innerHTML = data;
                        if (el.innerText.trim() == '')
                            data = null;

                        dotNetReference.invokeMethodAsync('EditorDataChanged', data);
                    });
                })
                .catch(error => console.error(error));
        },
        destroy(id) {
            editors[id].destroy()
                .then(() => delete editors[id])
                .catch(error => console.log(error));
        }
    };
})();