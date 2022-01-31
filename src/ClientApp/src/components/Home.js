import Components from './Components'
import React from 'react';
import RainAndDaylight from './RainAndDaylight'
import Sensor from './Sensor'
import Spinner from './Spinner'
import Logo from './Logo'
import Video from './Video'
import { Thermometer } from 'react-bootstrap-icons';
import { Moisture } from 'react-bootstrap-icons';
import { BrightnessHigh } from 'react-bootstrap-icons';

export default function Home(props) {
    return (
      <div>
        {
          props.isLoading || !props.latestWeatherReport.report ? 
            <Spinner />
            :
            <div className="container">
              <div className="row">
                <Logo />
              </div>
              <div className="row">
                <Sensor
                alertType="danger"
                icon={<Thermometer />}
                title="Temperature"
                href="temperature"
                unit="°C"
                value={props.latestWeatherReport.report.temperature}
                lastUpdated={props.latestWeatherReport.report.reportDate}
                maxValue={props.latestWeatherReport.records.temperatureMax}
                minValue={props.latestWeatherReport.records.temperatureMin}
                maxValueDate={props.latestWeatherReport.records.temperatureMaxDate}
                minValueDate={props.latestWeatherReport.records.temperatureMinDate}
                />

                <Sensor
                alertType="primary"
                icon={<Moisture />}
                title="Humidity"
                href="humidity"
                unit="%"
                value={props.latestWeatherReport.report.humidity}
                lastUpdated={props.latestWeatherReport.report.reportDate}
                maxValue={props.latestWeatherReport.records.humidityMax}
                minValue={props.latestWeatherReport.records.humidityMin}
                maxValueDate={props.latestWeatherReport.records.humidityMaxDate}
                minValueDate={props.latestWeatherReport.records.humidityMinDate}
                />

                <Sensor
                alertType="warning"
                title="UV Index"
                icon={<BrightnessHigh />}
                href="uv"
                value={props.latestWeatherReport.report.uv}
                lastUpdated={props.latestWeatherReport.report.reportDate}
                maxValue={props.latestWeatherReport.records.uvMax}
                minValue={props.latestWeatherReport.records.uvMin}
                maxValueDate={props.latestWeatherReport.records.uvMaxDate}
                minValueDate={props.latestWeatherReport.records.uvMinDate}
                />
            </div>

            <div className="row">
              <RainAndDaylight
              alertType="info"
              firstLightToday={props.latestWeatherReport.records.firstLightToday}
              lastLightToday={props.latestWeatherReport.records.lastLightToday}
              lastTimeRaining={props.latestWeatherReport.records.lastTimeRaining}
              raining={props.latestWeatherReport.report.raining}
              light={props.latestWeatherReport.report.light}
              />

              <Components />

              <Video />
            </div>
          </div>
        }
      </div>
    );
}
