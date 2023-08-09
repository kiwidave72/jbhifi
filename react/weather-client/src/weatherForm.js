import React from "react";
import { useState ,useRef,map} from "react";

function WeatherForm(){
    
    
    const SERVER_ENDPOINT = process.env.REACT_APP_SERVER_ENDPOINT;
    const CLIENT_APP_ID = process.env.REACT_APP_CLIENT_APP_ID;

    const [Weather, setWeather] = useState([]);
    const [ErrorMessage,setErrorMessage] = useState();

    const cityRef = useRef(null);
    const countryRef = useRef(null);
    
    const [city, setCity] = useState('');
    const [country, setCountry] = useState('');

    const fetchWeatherData = () => {

        setWeather([]);

        fetch(SERVER_ENDPOINT + "?appId="+CLIENT_APP_ID+ "&city="+city +"&country="+country)
          .then(response => {

            if (response.ok) {
                setErrorMessage();
                return response.json();
            }
            return Promise.reject(response); 
          })
          .then(data => {
            setWeather(data)
          })
          .catch((error) => {

            error.json().then((serverError) => {

               if(serverError.title=='One or more validation errors occurred.'){
                setErrorMessage('Check that City and Country have valid inputs.')

               }

            });            
           
          });
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
        <form >
            <p>Enter the city and country</p>
            <input type='text'  ref={cityRef} placeholder='city' id='city' onKeyDown={handleCityOnChange} />
            <input type='text' ref={countryRef} placeholder='country' id='country' onKeyDown={handleCountryOnChange}/>
            
            <button onClick={handleSubmit} >Submit</button>

            </form>

            <div>
            <p>{Weather.data?.description}
            </p>           
            <p>{Weather.errorMessage}</p>

            </div>
            <p>{ErrorMessage}</p>

        </div>
      
      );
}
  
export default WeatherForm;