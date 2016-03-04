using CeMaS.Common.Identity;
using CeMaS.Common.Units;
using CeMaS.Common.Validation;
using SenseLab.Common.Commands;
using SenseLab.Common.Objects;
using SenseLab.Common.Properties;
using SenseLab.Common.Values;
using Windows.Devices.Pwm;

namespace SenseLab.Devices.Pwm
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
                new IdentityInfo(string.Format($"Pin {number + 1}")),
                new ObjectType("PWM pin", new IdentityInfo("PWM pin", "Pulse width modulation pin")),
                parent: pwm
                )
        {
            pwm.ValidateNonNull(nameof(pwm));
            number.ValidateBetween(0, pwm.PinCount.Value);
            pin.ValidateNonNull(nameof(pin));
            Pwm = pwm;
            Pin = pin;

            Number = new Property<int>(this,
                nameof(Number), new IdentityInfo("Number"), number);
            Items.Add(Number);

            Items.Add(new Command(this,
                nameof(Close), new IdentityInfo("Close"),
                c => Close()
                ));

            IsStarted = new Property<bool>(this,
                nameof(IsStarted), new IdentityInfo("Is started"), false);
            Items.Add(IsStarted);
            Items.Add(new Command(this,
                nameof(Start), new IdentityInfo("Start"),
                c => Start(),
                () => CanStart
                ));
            Items.Add(new Command(this,
                nameof(Stop), new IdentityInfo("Stop"),
                c => Stop(),
                () => CanStop
                ));

            DutyCyclePercentage = new PhysicalProperty<double>(this,
                nameof(DutyCyclePercentage), new IdentityInfo("Duty cycle"), pin.GetActiveDutyCyclePercentage(), Units.Percentage
                );
            Items.Add(DutyCyclePercentage);
            Items.Add(new Command<double>(this,
                nameof(SetDutyCyclePercentage), new IdentityInfo("Set duty cycle"),
                (p, c) => SetDutyCyclePercentage(p),
                p => CanSetDutyCyclePercentage(p),
                parameters: new ValueInfo(DutyCyclePercentage)
                ));

            Polarity = new Property<PwmPulsePolarity>(this, nameof(Polarity), new IdentityInfo("Polarity"), pin.Polarity);
            Items.Add(Polarity);
            Items.Add(new Command<PwmPulsePolarity>(this,
                nameof(SetPolarity), new IdentityInfo("Set polarity"),
                (p, c) => SetPolarity(p),
                parameters: new ValueInfo(Polarity)
                ));
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
