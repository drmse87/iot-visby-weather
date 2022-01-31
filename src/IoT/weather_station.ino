#include <Adafruit_Sensor.h>
#include <DHT.h>
#include <DHT_U.h>
#include <LiquidCrystal_I2C.h>
#include <SPI.h>
#include <WiFiNINA.h>
#include <ArduinoHttpClient.h>
#include <ArduinoJson.h>
#include "arduino_secrets.h" 
#define DHTPIN 4
#define DHTTYPE    DHT22

// Set up WiFi.
char ssid[] = SECRET_SSID;
char pass[] = SECRET_PASS;
int keyIndex = 0;
int status = WL_IDLE_STATUS;
WiFiClient client;

// Set up HTTP.
char server[] = SECRET_SERVER;
String endpoint = SECRET_ENDPOINT;
String secretApiKey = SECRET_API_KEY;
int port = 80;
HttpClient http = HttpClient(client, server, port);

// Set up Temperature and humidity sensor.
DHT_Unified dht(DHTPIN, DHTTYPE);

// Set up LCD display.
LiquidCrystal_I2C lcd(0x27,20,4);

// Set up pins.
int lightPin = 2;
int rainPin = 7;
int uvPin = A2;

// Set up weather report (every 5 minutes) and LCD update interval (every second).
unsigned long previousMillisSendWeatherReport = 0; 
unsigned long previousMillisUpdateLcd = 0; 
const long sendWeatherReportInterval = 300000;
const long updateLcdInterval = 1000;

void setup() {
  // Initialize Serial device.
  Serial.begin(9600);

  // Set reference voltage lower (for UV sensor).
  analogReference(INTERNAL);

  // Initialize WiFi.
  checkForWifiModule();
  checkWifiFirmware();
  connectToWifi();
  printWifiStatus();

  // Initialize sensors.
  pinMode(lightPin, INPUT);
  pinMode(uvPin, INPUT);
  pinMode(rainPin, INPUT);

  // Initialize LCD. 
  lcd.init();
  lcd.backlight();
 
  // Prepare to print temperature and humidity details.
  dht.begin();
  Serial.println(F("DHTxx Unified Sensor Example"));
  sensor_t sensor;

  // Print temperature sensor details.
  dht.temperature().getSensor(&sensor);
  printTemperatureSensorDetails(sensor);

  // Print humidity sensor details.
  dht.humidity().getSensor(&sensor);
  printHumiditySensorDetails(sensor);
}

void printWifiStatus(){
  Serial.print(F("SSID: "));
  Serial.println(WiFi.SSID());

  IPAddress ip = WiFi.localIP();
  Serial.print(F("IP Address: "));
  Serial.println(ip);

  long rssi = WiFi.RSSI();
  Serial.print(F("Signal strength (RSSI):"));
  Serial.print(rssi);
  Serial.println(" dBm");
}

void connectToWifi(){
  while (status != WL_CONNECTED) {
    Serial.print(F("Attempting to connect to SSID: "));
    Serial.println(ssid);
    status = WiFi.begin(ssid, pass);

    delay(10000);
  }
  Serial.println(F("Connected to WiFi"));
}

void checkForWifiModule(){
  if (WiFi.status() == WL_NO_MODULE) {
    Serial.println(F("Communication with WiFi module failed!"));
    while (true);
  }
}

void checkWifiFirmware(){
  String fv = WiFi.firmwareVersion();
  if (fv < WIFI_FIRMWARE_LATEST_VERSION) {
    Serial.println(F("Please upgrade the firmware"));
  }
}

void printTemperatureSensorDetails(sensor_t sensor){  
  Serial.println(F("------------------------------------"));
  Serial.println(F("Temperature Sensor"));
  Serial.print  (F("Sensor Type: ")); Serial.println(sensor.name);
  Serial.print  (F("Driver Ver:  ")); Serial.println(sensor.version);
  Serial.print  (F("Unique ID:   ")); Serial.println(sensor.sensor_id);
  Serial.print  (F("Max Value:   ")); Serial.print(sensor.max_value); Serial.println(F("°C"));
  Serial.print  (F("Min Value:   ")); Serial.print(sensor.min_value); Serial.println(F("°C"));
  Serial.print  (F("Resolution:  ")); Serial.print(sensor.resolution); Serial.println(F("°C"));
  Serial.println(F("------------------------------------"));
}

void printHumiditySensorDetails(sensor_t sensor){
  Serial.println(F("Humidity Sensor"));
  Serial.print  (F("Sensor Type: ")); Serial.println(sensor.name);
  Serial.print  (F("Driver Ver:  ")); Serial.println(sensor.version);
  Serial.print  (F("Unique ID:   ")); Serial.println(sensor.sensor_id);
  Serial.print  (F("Max Value:   ")); Serial.print(sensor.max_value); Serial.println(F("%"));
  Serial.print  (F("Min Value:   ")); Serial.print(sensor.min_value); Serial.println(F("%"));
  Serial.print  (F("Resolution:  ")); Serial.print(sensor.resolution); Serial.println(F("%"));
  Serial.println(F("------------------------------------"));
}

void printLight(int light){
  Serial.print(F("Light: "));
  if (light == LOW) {
    Serial.println(F("True"));
    lcd.setCursor(7,1);
    lcd.print("L:Y");
  } else {
    Serial.println(F("False")); 
    lcd.setCursor(7,1);
    lcd.print("L:N");
  }
}

void printRaining(int rain){
  Serial.print(F("Raining: "));
  if (rain == LOW) {
    Serial.println(F("True"));
    lcd.setCursor(11,1);
    lcd.print("R:Y");
  } else {
    Serial.println(F("False"));
    lcd.setCursor(11,1);
    lcd.print("R:N");  
  }
}

void printUv(int uv){
  Serial.print(F("UV:"));
  Serial.println(uv);

  lcd.setCursor(7,0);
  lcd.print("UV:");
  lcd.print(uv);
}

void printWifiConnection(){
  if (status == WL_CONNECTED) {
    Serial.println(F("Connected to WiFi."));
    lcd.setCursor(13,0);
    lcd.print("W:Y");
  } else {
    lcd.setCursor(13,0);
    lcd.print("W:N");
  }
}

void printTemperature(float temperature){
  if (isnan(temperature)) {
    Serial.println(F("Error reading temperature!"));
  }
  else {
    lcd.setCursor(0,0);
    lcd.print(temperature);
    lcd.print("C");

    Serial.print(F("Temperature: "));
    Serial.print(temperature);
    Serial.println(F("°C"));
  }
}

void printHumidity(float humidity){
  if (isnan(humidity)) {
    Serial.println(F("Error reading humidity!"));
  }
  else {
    lcd.setCursor(0,1);
    lcd.print(humidity);   
    lcd.print("%");

    Serial.print(F("Humidity: "));
    Serial.print(humidity);
    Serial.println(F("%"));
  }
}

int readLightSensor(){
  return digitalRead(lightPin);
}

int readUvSensor(){
  return analogRead(uvPin);
}

int readRainSensor(){
  return digitalRead(rainPin);
}

float readTemperatureSensor(){
  sensors_event_t event;
  dht.temperature().getEvent(&event);

  return event.temperature;
}

float readHumiditySensor(){
  sensors_event_t event;
  dht.humidity().getEvent(&event);

  return event.relative_humidity;
}

DynamicJsonDocument getSensorValuesAsJsonDocument(){
  DynamicJsonDocument sensorValues(1024);

  sensorValues["temperature"] = readTemperatureSensor();
  sensorValues["humidity"] = readHumiditySensor();
  sensorValues["uv"] = readUvSensor();
  sensorValues["raining"] = readRainSensor();
  sensorValues["light"] = readLightSensor();

  return sensorValues;
}

void sendWeatherReport(){
  DynamicJsonDocument sensorValues = getSensorValuesAsJsonDocument();
  String jsonToPost;
  serializeJson(sensorValues, jsonToPost);

  http.beginRequest();
  http.post(endpoint);
  http.sendHeader("Content-Type", "application/json; charset=UTF-8");
  http.sendHeader("Content-Length", measureJson(sensorValues));
  http.sendHeader("X-Api-Key", secretApiKey);
  http.beginBody();
  http.print(jsonToPost);
  http.endRequest();
  
  int statusCode = http.responseStatusCode();
  String response = http.responseBody();

  Serial.print("Status code: ");
  Serial.println(statusCode);
  Serial.print("Response: ");
  Serial.println(response);
}

void loop() {
    // Based on the Blink without Delay example, sends weather report every 10 secs.
    unsigned long currentMillis = millis();
    if (currentMillis - previousMillisSendWeatherReport >= sendWeatherReportInterval) {
      previousMillisSendWeatherReport = currentMillis;

      sendWeatherReport();
    }

    if (currentMillis - previousMillisUpdateLcd >= updateLcdInterval) {
      previousMillisUpdateLcd = currentMillis;
      
      printLight(readLightSensor());
      printUv(readUvSensor());
      printRaining(readRainSensor());
      printTemperature(readTemperatureSensor());
      printHumidity(readHumiditySensor());
      printWifiConnection();
    }
}
