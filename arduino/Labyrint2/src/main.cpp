#include <Arduino.h>


bool ball_has_startet = false;
//bool ball_er_i_posisjon = false;

const int buttonPin = 10;
const int buttonLedPin = 9;
const int restartPin = 3;
const int IR_RECEIVE_PIN = 7;
const int buzzerPin = 11;
int state_obstacle  = 1; // sensor for motion/ obstacles
int pre_state_obstacle = 1;

bool goal = false;

void setup() {
 
  pinMode(buttonPin, INPUT_PULLUP);
  pinMode(buttonLedPin, OUTPUT); // light on button  
  pinMode(restartPin, INPUT);
  pinMode(IR_RECEIVE_PIN, INPUT_PULLUP);
  pinMode(buzzerPin, OUTPUT);

  Serial.begin(115200);
  
}

void loop() {

  state_obstacle = digitalRead(IR_RECEIVE_PIN);

  if (state_obstacle == 1 && pre_state_obstacle == 0){
     goal = true;
  }
  pre_state_obstacle  = state_obstacle;


  int button1State = digitalRead(buttonPin);
  if (button1State == LOW)
  // if pushing the botton, the button will light, and the ball has started
    {

      digitalWrite(buttonLedPin, HIGH);
      tone(buzzerPin, 800); // Send 1KHz sound signal...
      delay(600);        // ...for 1 sec
      noTone(buzzerPin);     // Stop sound...
      delay(600);        // ...for 1sec
      tone(buzzerPin, 800); // Send 1KHz sound signal...
      delay(600);        // ...for 1 sec
      noTone(buzzerPin);     // Stop sound...
      delay(600);        // ...for 1sec
      tone(buzzerPin, 800); // Send 1KHz sound signal...
      delay(600);        // ...for 1 sec
      noTone(buzzerPin);     // Stop sound...
      delay(600);        // ...for 1sec

      tone(buzzerPin, 1600); // Send 1KHz sound signal...
      delay(1500);        // ...for 1 sec
      noTone(buzzerPin);     // Stop sound...
      delay(1000);        // ...for 1sec

         
      Serial.print(button1State);
      ball_has_startet = true;

    }

    if ( ball_has_startet && goal){
      // send signal if sensor has registered obstacle
      Serial.print("s");
      goal = false;
    }

    if (button1State == HIGH)
    // turn off the light on the button
    {
      digitalWrite(buttonLedPin, LOW);
    }
    delay(100);

    

    
}





