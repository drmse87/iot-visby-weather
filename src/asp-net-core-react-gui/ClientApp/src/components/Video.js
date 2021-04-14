import React from 'react';
import { Film } from 'react-bootstrap-icons'

export default function Video(props) {
    return (
        <div className="col-md-4 mb-3">
            <div className="card bg-light h-100 overflow-auto">
                <div className="card-header"><Film /> Building a Weather Station</div>
                <div className="card-body">
                    <iframe 
                    width="308" 
                    height="187" 
                    src="https://www.youtube.com/embed/0aYAbL72kJo"
                    frameborder="0" 
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" 
                    allowfullscreen></iframe>
                </div>
            </div>
        </div>
    )
}