using Windows.Win32;
using Windows.Win32.System.Threading;

namespace Toolkit.Windows;

public class EfficiencyMode : 
    IEfficiencyMode
{
    public unsafe void SetProcessQualityOfServiceLevel(QualityOfService level)
    {
        PROCESS_POWER_THROTTLING_STATE powerThrottling = new PROCESS_POWER_THROTTLING_STATE
        {
            Version = PInvoke.PROCESS_POWER_THROTTLING_CURRENT_VERSION
        };

        switch (level)
        {
            case QualityOfService.Default:
                powerThrottling.ControlMask = 0;
                powerThrottling.StateMask = 0;
                break;

            case QualityOfService.Eco when Environment.OSVersion.Version >= new Version(11, 0):
            case QualityOfService.Low:
                powerThrottling.ControlMask = PInvoke.PROCESS_POWER_THROTTLING_EXECUTION_SPEED;
                powerThrottling.StateMask = PInvoke.PROCESS_POWER_THROTTLING_EXECUTION_SPEED;
                break;

            case QualityOfService.High:
                powerThrottling.ControlMask = PInvoke.PROCESS_POWER_THROTTLING_EXECUTION_SPEED;
                powerThrottling.StateMask = 0;
                break;

            default:
                throw new NotImplementedException();
        }

        _ = PInvoke.SetProcessInformation(
            hProcess: PInvoke.GetCurrentProcess(),
            ProcessInformationClass: PROCESS_INFORMATION_CLASS.ProcessPowerThrottling,
            ProcessInformation: &powerThrottling,
            ProcessInformationSize: (uint)sizeof(PROCESS_POWER_THROTTLING_STATE));
    }

    public unsafe void SetProcessPriorityClass(ProcessPriority priorityClass)
    {
        PROCESS_CREATION_FLAGS flags = priorityClass switch
        {
            ProcessPriority.Default => PROCESS_CREATION_FLAGS.NORMAL_PRIORITY_CLASS,
            ProcessPriority.Idle => PROCESS_CREATION_FLAGS.IDLE_PRIORITY_CLASS,
            ProcessPriority.BelowNormal => PROCESS_CREATION_FLAGS.BELOW_NORMAL_PRIORITY_CLASS,
            ProcessPriority.Normal => PROCESS_CREATION_FLAGS.NORMAL_PRIORITY_CLASS,
            ProcessPriority.AboveNormal => PROCESS_CREATION_FLAGS.ABOVE_NORMAL_PRIORITY_CLASS,
            ProcessPriority.High => PROCESS_CREATION_FLAGS.HIGH_PRIORITY_CLASS,
            ProcessPriority.Realtime => PROCESS_CREATION_FLAGS.REALTIME_PRIORITY_CLASS,
            _ => throw new NotImplementedException(),
        };

        _ = PInvoke.SetPriorityClass(
            hProcess: PInvoke.GetCurrentProcess(),
            dwPriorityClass: flags);
    }

    public void SetEfficiencyMode(bool value)
    {
        QualityOfService ecoLevel = Environment.OSVersion.Version >= new Version(11, 0)
            ? QualityOfService.Eco
            : QualityOfService.Low;

        SetProcessQualityOfServiceLevel(value ? ecoLevel : QualityOfService.Default);
        SetProcessPriorityClass(value ? ProcessPriority.Idle : ProcessPriority.Default);
    }
}