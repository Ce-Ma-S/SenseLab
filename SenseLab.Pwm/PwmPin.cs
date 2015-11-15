using CeMaS.Common.Validation;
using SenseLab.Common.Commands;
using SenseLab.Common.Objects;
using SenseLab.Common.Properties;
using Windows.Devices.Pwm;

namespace SenseLab.Pwm
{
    public class PwmPin :
        Object
    {
        public PwmPin(
            Pwm pwm,
            System.Guid id,
            int number
            ) :
            base(
                id,
                string.Format($"Pin {number + 1}"),
                new ObjectType("PWM pin", "PWM pin", "Pulse width modulation pin"),
                parent: pwm
                )
        {
            pwm.ValidateNonNull(nameof(pwm));
            number.ValidateIn(0, pwm.PinCount.Value);
            Pwm = pwm;

            Number = new Property<int>(this, nameof(Number), "Number", number);
            Items.Add(Number);

            IsOpen = new Property<bool>(this, nameof(IsOpen), "Is open", false);
            Items.Add(IsOpen);
            DelegateCommand command;
            command = new DelegateCommand(this, nameof(Open), "Open", () => Open(), () => CanOpen);
            Items.Add(command);
            command = new DelegateCommand(this, nameof(Close), "Close", () => Close(), () => CanClose);
            Items.Add(command);

            IsStarted = new Property<bool>(this, nameof(IsStarted), "Is started", false);
            Items.Add(IsStarted);
            command = new DelegateCommand(this, nameof(Start), "Start", () => Start(), () => CanStart);
            Items.Add(command);
            command = new DelegateCommand(this, nameof(Stop), "Stop", () => Stop(), () => CanStop);
            Items.Add(command);

            DutyCyclePercentage = new Property<double>(this, nameof(DutyCyclePercentage), "Duty cycle percentage");
            Items.Add(DutyCyclePercentage);
            command = new DelegateCommand<double>(this, nameof(SetDutyCyclePercentage), "Set duty cycle percentage", p => SetDutyCyclePercentage(p), p => CanSetDutyCyclePercentage(p));
            Items.Add(command);

            Polarity = new Property<PwmPulsePolarity>(this, nameof(Polarity), "Polarity");
            Items.Add(Polarity);
            command = new DelegateCommand<PwmPulsePolarity>(this, nameof(SetPolarity), "Set polarity", p => SetPolarity(p), p => CanSetPolarity);
            Items.Add(command);
        }

        public override bool IsAlive
        {
            get { return true; }
        }
        public Property<int> Number { get; }

        public Property<bool> IsOpen { get; }
        public bool CanOpen
        {
            get { return !IsOpen.Value; }
        }
        public void Open()
        {
            Pin = Pwm.Controller.OpenPin(Number.Value);
            IsOpen.Value = true;
            DutyCyclePercentage.Value = Pin.GetActiveDutyCyclePercentage();
            Polarity.Value = Pin.Polarity;
        }
        public bool CanClose
        {
            get { return IsOpen.Value; }
        }
        public void Close()
        {
            Pin.Dispose();
            Pin = null;
            IsOpen.Value = false;
            DutyCyclePercentage.HasValue = false;
            Polarity.HasValue = false;
        }

        public Property<bool> IsStarted { get; }
        public bool CanStart
        {
            get { return !IsStarted.Value; }
        }
        public void Start()
        {
            if (!IsOpen.Value)
                Open();
            Pin.Start();
            IsOpen.Value = true;
        }
        public bool CanStop
        {
            get { return IsStarted.Value; }
        }
        public void Stop()
        {
            Pin.Stop();
            IsOpen.Value = false;
        }

        public Property<double> DutyCyclePercentage { get; }
        public bool CanSetDutyCyclePercentage(double value)
        {
            return
                value >= 0 &&
                value <= 100 &&
                IsOpen.Value;
        }
        public void SetDutyCyclePercentage(double value)
        {
            Pin.SetActiveDutyCyclePercentage(value);
        }

        public Property<PwmPulsePolarity> Polarity { get; }
        public bool CanSetPolarity
        {
            get { return IsOpen.Value; }
        }
        public void SetPolarity(PwmPulsePolarity value)
        {
            Pin.Polarity = value;
        }

        protected Pwm Pwm { get; }
        protected Windows.Devices.Pwm.PwmPin Pin { get; private set; }
    }
}
