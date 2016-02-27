using CeMaS.Common.Identity;
using CeMaS.Common.Units;
using CeMaS.Common.Validation;
using SenseLab.Common.Commands;
using SenseLab.Common.Objects;
using SenseLab.Common.Properties;
using SenseLab.Common.Values;
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
                new ObjectType("PWM", new IdentityInfo("PWM", "Pulse width modulation")),
                parent
                )
        {
            controller.ValidateNonNull(nameof(controller));
            Controller = controller;

            PinCount = new Property<int>(this, nameof(PinCount), new IdentityInfo("Pin count"), Controller.PinCount);
            Items.Add(PinCount);

            Items.Add(new Command<int>(this,
                nameof(OpenPin),
                new IdentityInfo("Open pin"),
                (p, c) => OpenPin(p),
                p => CanOpenPin(p),
                parameters: "Number".ToValueInfo<int>(new IdentityInfo("Number", "Pin number"))
                ));

            Frequency = new PhysicalProperty<double>(this,
                nameof(Frequency),
                new IdentityInfo("Frequency"),
                Controller.ActualFrequency,
                Units.Hertz
                );
            Items.Add(Frequency);

            MinFrequency = new PhysicalProperty<double>(this,
                nameof(MinFrequency),
                new IdentityInfo("Minimum frequency"),
                Controller.MinFrequency,
                Units.Hertz
                );
            Items.Add(MinFrequency);

            MaxFrequency = new PhysicalProperty<double>(this,
                nameof(MaxFrequency),
                new IdentityInfo("Maximum frequency"),
                Controller.MaxFrequency,
                Units.Hertz
                );
            Items.Add(MaxFrequency);

            Items.Add(new Command<double>(this,
                nameof(SetFrequency),
                new IdentityInfo("Set frequency"),
                (p, c) => SetFrequency(p),
                p => CanSetFrequency(p),
                parameters: new ValueInfo(Frequency)
                ));
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
            number.ValidateBetween(0, PinCount.Value - 1, nameof(number));
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
