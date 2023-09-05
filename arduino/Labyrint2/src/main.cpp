#include <Arduino.h>

const int buttonPin = 10;
const int buttonLedPin = 9;
const int IR_RECEIVE_PIN = 7;
const int buzzerPin = 11;
int state_obstacle  = 1; // sensor for motion/ obstacles
int pre_state_obstacle = 1;

bool game_running = false;
bool goal = false;
int number_of_goals = 0;

void setup() {
 
  pinMode(buttonPin, INPUT_PULLUP);
  pinMode(buttonLedPin, OUTPUT); // light on button  
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

  // if pushing the botton, the button will light, and the ball has started
  if (button1State == LOW && !game_running)
  {

    digitalWrite(buttonLedPin, HIGH);        
    Serial.print(button1State);
    game_running = true;

  }
  else if ( game_running && goal){
    // send signal if sensor has registered obstacle
    Serial.print("s");
    goal = false;
    number_of_goals += 1;
    if (number_of_goals >= 2) {
      game_running = false;
    }
  }
  else if (button1State == HIGH)
  {
  // turn off the light on the button
    digitalWrite(buttonLedPin, LOW);
  }
  delay(20);

}
