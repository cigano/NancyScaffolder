using EnvDTE;

namespace NancyScaffolder.PrimaryKeyLocators
{
    public interface IPrimaryKeyLocator
    {
        bool IsPrimaryKey(CodeClass codeClass, CodeProperty codeProperty);
    }
}
