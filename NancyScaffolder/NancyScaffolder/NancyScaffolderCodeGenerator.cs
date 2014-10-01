using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using NancyScaffolder.UI;
using Microsoft.AspNet.Scaffolding;
using System.Collections.Generic;

namespace NancyScaffolder
{
    public class NancyScaffolderCodeGenerator : CodeGenerator
    {
        readonly CustomViewModel _viewModel;

        /// <summary>
        /// Constructor for the custom code generator
        /// </summary>
        /// <param name="context">Context of the current code generation operation based on how scaffolder was invoked(such as selected project/folder) </param>
        /// <param name="information">Code generation information that is defined in the factory class.</param>
        public NancyScaffolderCodeGenerator(
            CodeGenerationContext context,
            CodeGeneratorInformation information)
            : base(context, information)
        {
            _viewModel = new CustomViewModel(Context);
        }


        /// <summary>
        /// Any UI to be displayed after the scaffolder has been selected from the Add Scaffold dialog.
        /// Any validation on the input for values in the UI should be completed before returning from this method.
        /// </summary>
        /// <returns></returns>
        public override bool ShowUIAndValidate()
        {
            // Bring up the selection dialog and allow user to select a model type
            var window = new SelectModelWindow(_viewModel);
            bool? showDialog = window.ShowDialog();
            return showDialog ?? false;
        }

        /// <summary>
        /// This method is executed after the ShowUIAndValidate method, and this is where the actual code generation should occur.
        /// In this example, we are generating a new file from t4 template based on the ModelType selected in our UI.
        /// </summary>
        public override void GenerateCode()
        {
            PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));

            // Get the selected code type
            var codeType = _viewModel.SelectedModelType.CodeType;

            // Setup the scaffolding item creation parameters to be passed into the T4 template.
            var parameters = new Dictionary<string, object>()
            {
                { 
                    /* This value should match the parameter in T4 */
                    "ModelType", 
                    
                    /* This is the value passed */ 
                    codeType
                }

                //You can pass more parameters after they are defined in the template
            };

            // Add the custom scaffolding item from T4 template.
            AddFileFromTemplate(Context.ActiveProject,
                "Modules\\" + ps.Pluralize(_viewModel.SelectedModelType.ShortTypeName) + "Module",
                "ModuleTemplate",
                parameters,
                skipIfExists: false);

            AddFolder(Context.ActiveProject, "Views\\" + ps.Pluralize(_viewModel.SelectedModelType.ShortTypeName));

            AddFileFromTemplate(Context.ActiveProject,
                "Views\\" + ps.Pluralize(_viewModel.SelectedModelType.ShortTypeName) + "\\Index",
                "IndexViewTemplate",
                parameters,
                skipIfExists: false);

            AddFileFromTemplate(Context.ActiveProject,
                "Views\\" + ps.Pluralize(_viewModel.SelectedModelType.ShortTypeName) + "\\Create",
                "CreateViewTemplate",
                parameters,
                skipIfExists: false);

            AddFileFromTemplate(Context.ActiveProject,
                "Views\\" + ps.Pluralize(_viewModel.SelectedModelType.ShortTypeName) + "\\Edit",
                "EditViewTemplate",
                parameters,
                skipIfExists: false);

            AddFileFromTemplate(Context.ActiveProject,
                "Views\\" + ps.Pluralize(_viewModel.SelectedModelType.ShortTypeName) + "\\Details",
                "DetailsViewTemplate",
                parameters,
                skipIfExists: false);
        }
    }
}
