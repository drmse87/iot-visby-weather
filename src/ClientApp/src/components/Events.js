import React from 'react';
import Spinner from './Spinner'

export default function Events(props) {
    return (
      <div className="text-center">
        {
          props.isLoading ?
            <Spinner />
            :
            <div>
              <div className="alert alert-info" role="alert">
                <p>
                  There has been a total of {props.raining.length} raining events, 
                  out of a total of {props.allWeatherReports.length} weather reports.
                  This means that it rains {((props.raining.length / props.allWeatherReports.length) * 100).toFixed(2)}% of the time.
                </p>
              </div>

              <div className="alert alert-warning" role="alert">
                <p>
                  There has been a total of {props.light.length} light events, out of a total of {props.allWeatherReports.length} weather reports.
                  This means that it is light {((props.light.length / props.allWeatherReports.length) * 100).toFixed(2)}% of the time.
                </p>
              </div>
            </div>
        }
      </div>
    );
}
