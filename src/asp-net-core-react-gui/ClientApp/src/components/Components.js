import React from 'react';
import { Check } from 'react-bootstrap-icons';
import { Tools } from 'react-bootstrap-icons';

export default function Components() {
    return (
        <div className="col-md-4 mb-3">
            <div className="card bg-light h-100">
                <div className="card-header"><Tools /> Components used</div>
                <div className="card-body">
                    <p className="card-text">
                        <ul className="list-unstyled">
                            <li>
                                <small>
                                    <Check /> Arduino UNO WiFi Rev.2
                                </small>
                            </li>
                            <li>
                                <small>
                                    <Check /> Light Sensor (LM393)
                                </small>
                            </li>
                            <li>
                                <small>
                                    <Check /> LCD Display (HD44780)
                                </small>
                            </li>
                            <li>
                                <small>
                                    <Check /> Raindrop Sensor (MH-RD)
                                </small>
                            </li>
                            <li>
                                <small>
                                    <Check /> Temperature/Humidity Sensor (RHT03/DHT22)
                                </small>
                            </li>
                            <li>
                                <small>
                                    <Check /> UV Sensor (SENS-43UV)
                                </small>
                            </li>
                        </ul>
                    </p>
                </div>
            </div>
        </div>
    )
}