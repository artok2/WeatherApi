# WeatherApi

Scheduled azure Weather_function that polls OpenWeatherMap’s API hourly and saves temperature (in Celsius) in Turku and timestamp to Azure DB storage. Contains weather web api where you provide date and time (UTC/GMT) as parameter and get temperature, humidity and pressure for specified time as result using previously stored values.