namespace Toolkit.Windows
{
    public interface IEfficiencyMode
    {
        void SetEfficiencyMode(bool value);
        void SetProcessPriorityClass(ProcessPriority priorityClass);
        void SetProcessQualityOfServiceLevel(QualityOfService level);
    }
}