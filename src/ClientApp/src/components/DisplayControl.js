import React from 'react';

export default function DisplayControl(props) {
    return (
        <div className="btn-group" role="group" aria-label="Display control">
            <button 
            type="button" 
            onClick={() => props.changeDisplay("hourly")} className="btn btn-secondary"
            disabled={props.display === 'hourly' ? true : false}>
                Hourly
            </button>
            
            <button 
            type="button" 
            onClick={() => props.changeDisplay("daily")} className="btn btn-secondary"
            disabled={props.display === 'daily' ? true : false}>
                Daily
            </button>

            <button 
            type="button" 
            onClick={() => props.changeDisplay("monthly")} 
            className="btn btn-secondary"
            disabled={props.display === 'monthly' ? true : false}>
                Monthly average
            </button>

            <button 
            type="button" 
            onClick={() => props.changeDisplay("yearly")} 
            className="btn btn-secondary"
            disabled={props.display === 'yearly' ? true : false}>
                Yearly average
            </button>
        </div>
    )
}