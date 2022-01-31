import React from 'react';
import Spinner from './Spinner'
import DisplayControl from './DisplayControl'
import { Line } from "react-chartjs-2"

export default function SensorGraph(props) {
  return (
    <div className="text-center">
      {
        props.isLoading ? 
          <Spinner />
          :
          <Line 
          data={
              {
                  labels: props.labels, 
                  datasets: [{
                      data: props.data,
                      label: props.label,
                      borderColor: props.color,
                      fill: true
                      }]
              }} 
          options=
              {{
                  legend: {
                      display: false
                  }
              }}
          />
      }

      <DisplayControl
      changeDisplay={props.changeDisplay}
      display={props.display}     
      />

    </div>
  )
}

