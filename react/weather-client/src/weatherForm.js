import React from "react";
import { useState ,useRef} from "react";

function WeatherForm(){
    
    
    const SERVER_ENDPOINT = process.env.REACT_APP_SERVER_ENDPOINT;
    const CLIENT_APP_ID = process.env.REACT_APP_CLIENT_APP_ID;

    const [Weather, setWeather] = useState([]);

    const cityRef = useRef(null);
    const countryRef = useRef(null);
    
    const [city, setCity] = useState('');
    const [country, setCountry] = useState('');

    const fetchWeatherData = () => {

        setWeather([]);

        fetch(SERVER_ENDPOINT + "?appId="+CLIENT_APP_ID+ "&city="+city +"&country="+country)
          .then(response => {
            return response.json()
          })
          .then(data => {
            setWeather(data)
          })
      }

    const handleSubmit = (event) => {
        event.preventDefault();
        fetchWeatherData();
      };

      const handleCityOnChange = (event) => {
            setCity(cityRef.current.value);
        
      };
      const handleCountryOnChange = (event) => {
            setCountry(countryRef.current.value);
      };


    return(

        <div>


        <p> {city} {country}  </p>
        <form >
            <p>Enter the city and country</p>
            <input type='text'  ref={cityRef} placeholder='city' id='city' onKeyDown={handleCityOnChange} />
            <input type='text' ref={countryRef} placeholder='country' id='country' onKeyDown={handleCountryOnChange}/>
            
            <button onClick={handleSubmit} >Submit</button>

            </form>

            <div>
            <p> {Weather.description}  </p>
            </div>

        </div>
    );
}
  
export default WeatherForm;