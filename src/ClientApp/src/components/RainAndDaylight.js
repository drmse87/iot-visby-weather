import React from 'react';
import { Umbrella } from 'react-bootstrap-icons';
import { Moon } from 'react-bootstrap-icons';
import { Sun } from 'react-bootstrap-icons';
import { Cloud } from 'react-bootstrap-icons';
import moment from 'moment'

export default function RainAndDaylight(props) {
    return (
        <div className="col-md-4 mb-3" id="rain-daylight">
            <div className={`h-100 alert alert-${props.alertType}`} role="alert">
                <h4 className="alert-heading">
                {
                    props.raining 
                    ? 
                    (
                        <span><Umbrella /> Raining</span>
                    )
                    :
                    (
                        <span><Cloud /> Not raining</span>
                    )
                }
                </h4>

                <p>
                    It is {props.raining ? "" : "not"} raining right now.
                    {" "}
                    {
                        props.lastTimeRaining 
                        && 
                        (
                            <span 
                            title={moment(props.lastTimeRaining).format("YYYY-MM-DD HH:mm:ss")}
                            >
                                Last time it rained was {moment(props.lastTimeRaining).fromNow()}
                            </span>
                        )
                    }
                </p>

                <hr />

                <h4 className="alert-heading">
                {
                    props.light 
                    ? 
                    (
                        <span><Sun /> Day</span>
                    )
                    :
                    (
                        <span><Moon /> Night</span>
                    )
                }              
                </h4>

                <p>
                    It is currently {props.light ? "light" : "dark"} outside.
                    {" "}
                    {
                    props.firstLightToday && 
                    <span>
                        It got light outside today at {props.firstLightToday}. 
                        Last time it was light was {props.lastLightToday}.
                        Maybe that's the last we'll see of the sun today?
                    </span>
                }
                </p>
            </div>
        </div>
    );
}
