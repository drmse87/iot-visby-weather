import React from 'react'

export default function Logo() {
    return (
        <div className="col">
            <div className="jumbotron jumbotron-fluid">
                <div className="container">
                    <h1 className="display-4">Visby Weather</h1>

                    <hr className="my-4" />

                    <p className="lead">
                        Built with <a href="https://arduino.cc">Arduino UNO WiFi Rev.2</a>, <a href="https://asp.net">ASP.NET Core</a>, <a href="https://reactjs.org">React.js</a>, <a href="https://www.chartjs.org/">Charts.js</a> and <a href="https://getbootstrap.com">Bootstrap</a> by dm222ai
                    </p>
                </div>
            </div>
        </div>
    )
}