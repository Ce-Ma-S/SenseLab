using CeMaS.Common.Units;
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
        internal PwmPin(
            Pwm pwm,
            string id,
            int number,
            Windows.Devices.Pwm.PwmPin pin
            ) :
            base(
                pwm.Environment,
                id,
                string.Format($"Pin {number + 1}"),
                new ObjectType("PWM pin", "PWM pin", "Pulse width modulation pin"),
                parent: pwm
                )
        {
            pwm.ValidateNonNull(nameof(pwm));
            number.ValidateIn(0, pwm.PinCount.Value);
            pin.ValidateNonNull(nameof(pin));
            Pwm = pwm;
            Pin = pin;

            Number = new Property<int>(this,
                nameof(Number), "Number", number);
            Items.Add(Number);

            DelegateCommand command;
            command = new DelegateCommand(this,
                nameof(Close), "Close",
                () => Close()
                );
            Items.Add(command);

            IsStarted = new Property<bool>(this,
                nameof(IsStarted), "Is started", false);
            Items.Add(IsStarted);
            command = new DelegateCommand(this,
                nameof(Start), "Start",
                () => Start(),
                () => CanStart
                );
            Items.Add(command);
            command = new DelegateCommand(this,
                nameof(Stop), "Stop",
                () => Stop(),
                () => CanStop
                );
            Items.Add(command);

            DutyCyclePercentage = new PhysicalProperty<double>(this,
                nameof(DutyCyclePercentage), "Duty cycle", pin.GetActiveDutyCyclePercentage(), Units.Percentage
                );
            Items.Add(DutyCyclePercentage);
            command = new DelegateCommand<double>(this,
                nameof(SetDutyCyclePercentage), "Set duty cycle",
                p => SetDutyCyclePercentage(p),
                p => CanSetDutyCyclePercentage(p),
                parameters: new CommandPhysicalParameterInfo<double>(DutyCyclePercentage)
                );
            Items.Add(command);

            Polarity = new Property<PwmPulsePolarity>(this, nameof(Polarity), "Polarity", pin.Polarity);
            Items.Add(Polarity);
            command = new DelegateCommand<PwmPulsePolarity>(this,
                nameof(SetPolarity), "Set polarity",
                p => SetPolarity(p),
                parameters: new CommandParameterInfo<PwmPulsePolarity>(Polarity)
                );
            Items.Add(command);
        }

        public override bool IsAlive
        {
            get { return true; }
        }
        public Property<int> Number { get; }

        public void Close()
        {
            Pin.Dispose();
            Pin = null;
            Pwm.Children.Remove(this);
        }

        public Property<bool> IsStarted { get; }
        public bool CanStart
        {
            get { return !IsStarted.Value; }
        }
        public void Start()
        {
            Pin.Start();
            IsStarted.Value = true;
        }
        public bool CanStop
        {
            get { return IsStarted.Value; }
        }
        public void Stop()
        {
            Pin.Stop();
            IsStarted.Value = false;
        }

        public PhysicalProperty<double> DutyCyclePercentage { get; }
        public bool CanSetDutyCyclePercentage(double value)
        {
            return
                value >= 0 &&
                value <= 100;
        }
        public void SetDutyCyclePercentage(double value)
        {
            Pin.SetActiveDutyCyclePercentage(value);
            DutyCyclePercentage.Value = Pin.GetActiveDutyCyclePercentage();
        }

        public Property<PwmPulsePolarity> Polarity { get; }
        public void SetPolarity(PwmPulsePolarity value)
        {
            Pin.Polarity = value;
            Polarity.Value = value;
        }

        protected Pwm Pwm { get; }
        protected Windows.Devices.Pwm.PwmPin Pin { get; private set; }
    }
}
