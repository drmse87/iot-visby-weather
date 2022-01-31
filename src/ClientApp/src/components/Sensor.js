import React from 'react';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import moment from 'moment'

export default function Sensor(props) {
    return (
        <div className="col-md-4 mb-3" id={props.href}>
            <div className={`alert h-100 alert-${props.alertType}`} role="alert">
                <h4 className="alert-heading">{props.icon} {props.title}</h4>
                <div className="text-center">
                    <h1 className="display-5">{props.value}{props.unit}</h1>   
                </div>
                <p className="card-text">
                    <small 
                    className="text-muted"
                    title={moment(props.lastUpdated).format("YYYY-MM-DD HH:mm:ss")}>
                        Last updated {moment(props.lastUpdated).fromNow()}
                    </small>
                </p>

                <hr />

                <ul className="list-group list-group-horizontal">
                    <li className={`list-group-item list-group-item-${props.alertType}`}>
                        <strong>MAX: </strong>{props.maxValue}{props.unit} set {" "}
                        <span title={ moment(props.maxValueDate).format("YYYY-MM-DD HH:mm:ss") }>
                            { moment(props.maxValueDate).fromNow() }
                        </span>
                    </li>
                    <li className={`list-group-item list-group-item-${props.alertType}`}>
                        <strong>MIN: </strong>{props.minValue}{props.unit} set {" "}
                        <span title={ moment(props.minValueDate).format("YYYY-MM-DD HH:mm:ss") }>
                            {moment(props.minValueDate).fromNow() }
                        </span>
                    </li>
                </ul>
                <NavLink 
                tag={Link}
                to={props.href} 
                className={`w-100 m-1 p-1 align-bottom btn input-block-level btn-outline-${props.alertType} btn-sm`}>
                    More {props.title} readings..</NavLink>
            </div>
        </div>
    );
}
