using CeMaS.Common.Units;
using CeMaS.Common.Validation;
using SenseLab.Common.Commands;
using SenseLab.Common.Objects;
using SenseLab.Common.Properties;
using System.Linq;
using Windows.Devices.Pwm;

namespace SenseLab.Devices.Pwm
{
    public class Pwm :
        Object
    {
        public Pwm(
            ObjectEnvironment environment,
            string id,
            IdentityInfo info,
            PwmController controller,
            Object parent = null
            ) :
            base(
                environment, id, info,
                new ObjectType("PWM", "PWM", "Pulse width modulation"),
                parent
                )
        {
            controller.ValidateNonNull(nameof(controller));
            Controller = controller;

            PinCount = new Property<int>(this, nameof(PinCount), "Pin count", Controller.PinCount);
            Items.Add(PinCount);

            DelegateCommand command;
            command = new DelegateCommand<int>(this,
                nameof(OpenPin), "Open pin",
                p => OpenPin(p),
                p => CanOpenPin(p),
                parameters: new CommandParameterInfo<int>("Number", "Number", "Pin number")
                );
            Items.Add(command);

            Frequency = new PhysicalProperty<double>(this,
                nameof(Frequency), "Frequency", Controller.ActualFrequency, Units.Hertz);
            Items.Add(Frequency);

            MinFrequency = new PhysicalProperty<double>(this,
                nameof(MinFrequency), "Minimum frequency", Controller.MinFrequency, Units.Hertz);
            Items.Add(MinFrequency);

            MaxFrequency = new PhysicalProperty<double>(this,
                nameof(MaxFrequency), "Maximum frequency", Controller.MaxFrequency, Units.Hertz);
            Items.Add(MaxFrequency);

            command = new DelegateCommand<double>(this,
                nameof(SetFrequency), "Set frequency",
                p => SetFrequency(p),
                p => CanSetFrequency(p),
                parameters: new CommandPhysicalParameterInfo<double>(Frequency)
                );
            Items.Add(command);
        }

        public override bool IsAlive
        {
            get { return true; }
        }
        public Property<int> PinCount { get; }

        public bool CanOpenPin(int number)
        {
            return
                number >= 0 &&
                number < PinCount.Value &&
                GetPin(number) == null;
        }
        public PwmPin OpenPin(int number)
        {
            number.ValidateIn(0, PinCount.Value - 1, nameof(number));
            var pin = GetPin(number);
            if (pin == null)
            {
                pin = new PwmPin(this,
                    string.Format($"Pin{number}"),
                    number,
                    Controller.OpenPin(number)
                    );
                Children.Add(pin);
            }
            return pin;
        }

        public PhysicalProperty<double> Frequency { get; }
        public PhysicalProperty<double> MinFrequency { get; }
        public PhysicalProperty<double> MaxFrequency { get; }
        public bool CanSetFrequency(double value)
        {
            return
                value >= MinFrequency.Value &&
                value <= MaxFrequency.Value;
        }
        public void SetFrequency(double value)
        {
            Controller.SetDesiredFrequency(value);
            Frequency.Value = Controller.ActualFrequency;
        }

        protected PwmPin GetPin(int number)
        {
            return Children.
                Cast<PwmPin>().
                SingleOrDefault(i => i.Number.Value == number);
        }

        protected PwmController Controller { get; }
    }
}
