import React, { useState, useEffect } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import Events from './components/Events';
import Home from './components/Home';
import './custom.css'
import useInterval from 'use-interval'
import SensorGraph from './components/SensorGraph'

export default function App() {
  const [allWeatherReports, setAllWeatherReports] = useState([])
  const [display, setDisplay] = useState("daily")
  const [humidity, setHumidity] = useState([])
  const [latestWeatherReport, setLatestWeatherReport] = useState({})
  const [light, setLight] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [raining, setRaining] = useState([])
  const [temperature, setTemperature] = useState([])
  const [uv, setUv] = useState([])

  useInterval(async () => {
    fetchResources()
  }, 60000);

  const changeDisplay = (newDisplay) => {
    setDisplay(newDisplay)
  }

  const fetchResources = async () => {
    const [fetchedAllWeatherReports, fetchedLatestWeatherReport, fetchedTemperature, fetchedHumidity, fetchedUv, 
      fetchedRaining, fetchedLight ] = await Promise.all([
      fetch("/api/v1/weatherreports"),
      fetch("/api/v1/weatherreports/latest"),
      fetch(`/api/v1/weatherreports/temperature?display=${display}`),
      fetch(`/api/v1/weatherreports/humidity?display=${display}`),
      fetch(`/api/v1/weatherreports/uv?display=${display}`),
      fetch("/api/v1/weatherreports/raining"),
      fetch("/api/v1/weatherreports/light")
    ])

    const [allWeatherReportsJson, latestWeatherReportJson, temperatureJson, humidityJson, 
      uvJson, rainingJson, lightJson] = await Promise.all([
      fetchedAllWeatherReports.json(),
      fetchedLatestWeatherReport.json(),
      fetchedTemperature.json(),
      fetchedHumidity.json(),
      fetchedUv.json(),
      fetchedRaining.json(),
      fetchedLight.json()
    ])

    if (fetchedLatestWeatherReport.ok) {
      setAllWeatherReports(allWeatherReportsJson)
      setLatestWeatherReport(latestWeatherReportJson)
      setTemperature(temperatureJson)
      setHumidity(humidityJson)
      setUv(uvJson)
      setRaining(rainingJson)
      setLight(lightJson)
      setIsLoading(false)
    }
  }

  useEffect(() => {
    fetchResources()
  }, [display])

  return (
      <Layout>
        <Route exact path='/' render={(props) => 
          (<Home 
          isLoading={isLoading} 
          latestWeatherReport={latestWeatherReport} 
          {...props} /> 
        )} />

        <Route path='/temperature' render={(props) => 
          (<SensorGraph 
          changeDisplay={changeDisplay}
          color="red"
          data={temperature.map(t => t.temperature)}
          display={display}
          label="Temperature"
          labels={temperature.map(t => t.reportDate)}
          isLoading={isLoading} 
          {...props} /> 
        )} />

        <Route path='/humidity' render={(props) => 
          (<SensorGraph 
          changeDisplay={changeDisplay}
          color="blue"
          data={humidity.map(t => t.humidity)}
          display={display}
          isLoading={isLoading} 
          label="Humidity"
          labels={humidity.map(t => t.reportDate)}
          {...props} /> 
        )} />

        <Route path='/uv' render={(props) => 
          (<SensorGraph 
          changeDisplay={changeDisplay}
          color="yellow"
          data={uv.map(t => t.uv)}
          display={display}
          isLoading={isLoading} 
          label="UV Index"
          labels={uv.map(t => t.reportDate)}
          {...props} 
          />
        )} />

        <Route path='/events' render={(props) => 
          (<Events
            allWeatherReports={allWeatherReports}
            isLoading={isLoading}
            light={light} 
            raining={raining} 
            {...props}
            />
        )} />

      </Layout>
    );
}
