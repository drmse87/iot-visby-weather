# Description
I have built an IoT weather station using Arduino Uno WiFi Rev.2, ASP.NET Core, React.js, Charts.js and Bootstrap.css. The weather station can be accessed [here](https://iot.sommarvind.tech/), there is also a [YouTube video](https://youtube.com/watch?v=0aYAbL72kJo). Building a weather station has made me realize two things: first of all, that it is hard to measure temperature (requires shade) and UV (requires sun) from the same location. Secondly, the rain sensor is a bit "dumb" in the sense that it will report that it is raining for as long as it remains wet. This means that it will accurately report when it rains, but inaccurately report when it has stopped raining (it takes some time for the sensor to dry).

The following sensors were used:

* Light Sensor (LM393)
* LCD Display (HD44780)
* Raindrop Sensor (MH-RD)
* Temperature/Humidity Sensor (RHT03/DHT22)
* UV Sensor (SENS-43UV)

Every five minutes, the Arduino Uno reads the sensor values and POSTs them using the ArduinoJson library to my ASP.NET Core API (this endpoint is protected with an API key to prevent unauthorized access). The results are then presented in a React.js/Charts.js frontend. I would say that I have learned quite a lot about Arduino (and C++) during this project. Connecting to the WiFi was never a problem, instead, the challenge was getting the data from the Arduino to the API. For some reason, some of my HTTP requests timed out. I tried several different HTTP libraries and WebSockets, to no avail. Finally, I managed to pinpoint the error to the Arduino WiFiSSLClient, i.e HTTPS request on port 443. "Regular" HTTP requests on port 80 never timed out. Therefore, I made an exception in the HTTPS enforcement in my ASP application for Arduino User-Agents (it also checks the API key to verify the request is coming from the IoT device and not some intruder). 