using CeMaS.Common.Validation;
using SenseLab.Common.Objects;
using SenseLab.Common.Properties;
using Windows.Devices.Pwm;

namespace SenseLab.Pwm
{
    public class Pwm :
        Object
    {
        public Pwm(
            System.Guid id,
            string name,
            PwmController controller,
            string description = null,
            IObject parent = null
            ) :
            base(
                id, name,
                new ObjectType("PWM", "PWM", "Pulse width modulation"),
                description, parent
                )
        {
            controller.ValidateNonNull(nameof(controller));
            Controller = controller;

            PinCount = new Property<int>(this, nameof(PinCount), "Pin count", Controller.PinCount);
            Items.Add(PinCount);

            for (int i = 0; i < PinCount.Value; i++)
            {
                Children.Add(new PwmPin(
                    this,
                    System.Guid.NewGuid(),
                    i
                    ));
            }
        }

        public override bool IsAlive
        {
            get { return true; }
        }
        public Property<int> PinCount { get; }

        protected internal PwmController Controller { get; }
    }
}
