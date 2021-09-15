namespace Robotics.Controllers
{
    public sealed class DroneController : Controller
    {
        // NOTE: The Arduino/low level C-like code is intentional. The purpose of this project is to simulate making an Arduino-based quadcopter in Unity.

        [UnityEngine.Range(1000, 2000)]
        public int throttle = 1500;

        const byte FR = 0;
        const byte BR = 1;
        const byte BL = 2;
        const byte FL = 3;

        Servo[] motors = new Servo[4];

        void Start()
        {
            Servo.attach(out motors[FR], GetComponent<Servo>(1, FR));
            Servo.attach(out motors[BR], GetComponent<Servo>(1, BR));
            Servo.attach(out motors[BL], GetComponent<Servo>(1, BL));
            Servo.attach(out motors[FL], GetComponent<Servo>(1, FL));
        }

        void Update()
        {
            for (byte i = 0; i < 4; i++)
            {
                motors[i].writeMicroseconds(throttle);
            }
        }
    }
}
