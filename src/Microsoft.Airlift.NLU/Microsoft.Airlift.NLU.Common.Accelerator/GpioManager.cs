using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;

namespace Microsoft.Airlift.NLU.Common.Accelerator
{
    public sealed class GpioManager
    {
        private const int LED_PIN = 5;
        private const int LIGHT_PIN = 6;
        private const int BUTTON_PIN = 18;

        private GpioPin ledPin = null;
        private GpioPin lightPin = null;
        private GpioPin buttonPin = null;
        private GpioController gpioController;

        private object isButtonPressedLock = new object();
        private bool isButtonPressed = false;


        private DispatcherTimer timer;

        public event EventHandler<EventArgs> buttonPressed;
        public event EventHandler<EventArgs> buttonReleased;

        public void InitGpio()
        {
            if (gpioController != null)
                Dispose();

            gpioController = GpioController.GetDefault();

            if (gpioController == null)
                return;

            ledPin = gpioController.OpenPin(LED_PIN);
            ledPin.Write(GpioPinValue.High);
            ledPin.SetDriveMode(GpioPinDriveMode.Output);

            lightPin = gpioController.OpenPin(LIGHT_PIN);
            lightPin.Write(GpioPinValue.High);
            lightPin.SetDriveMode(GpioPinDriveMode.Output);

            buttonPin = gpioController.OpenPin(BUTTON_PIN);
            buttonPin.SetDriveMode(GpioPinDriveMode.Input);

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(1000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            var buttonValue = buttonPin.Read();

            if (isButtonPressed && buttonValue == GpioPinValue.Low)
            {
                lock (isButtonPressedLock)
                {
                    if (!isButtonPressed) return;
                    isButtonPressed = false;
                }

                if (buttonReleased != null)
                    buttonReleased.Invoke(this, new EventArgs());

                return;
            }
            if (!isButtonPressed && buttonValue == GpioPinValue.High)
            {
                lock (isButtonPressedLock)
                {
                    if (isButtonPressed) return;
                    isButtonPressed = true;
                }

                if (buttonPressed != null)
                    buttonPressed.Invoke(this, new EventArgs());

                return;
            }
        }



        public void TurnLightOn()
        {
            if (gpioController == null)
                return;
            lightPin.Write(GpioPinValue.Low);
        }

        public void TurnLightOff()
        {
            if (gpioController == null)
                return;

            lightPin.Write(GpioPinValue.High);
        }

        public void TurnLedOn()
        {
            if (gpioController == null)
                return;
            ledPin.Write(GpioPinValue.Low);
        }

        public void TurnLedOff()
        {
            if (gpioController == null)
                return;

            ledPin.Write(GpioPinValue.High);
        }

        public bool IsButtonPressed()
        {
            if (gpioController == null)
                return false;

            return buttonPin.Read() == GpioPinValue.High;
        }

        public void Dispose()
        {
            if (gpioController == null)
                return;

            if (ledPin != null)
            {
                ledPin.Dispose();
                ledPin = null;
            }

            if (lightPin != null)
            {
                lightPin.Dispose();
                lightPin = null;
            }

            if (buttonPin != null)
            {
                buttonPin.Dispose();
                buttonPin = null;
            }
        }
    }
}
