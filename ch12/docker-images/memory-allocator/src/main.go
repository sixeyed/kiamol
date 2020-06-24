package main

import (
    "fmt"
    "time"
	"github.com/spf13/viper"
)

type Configuration struct {
	Interval int
	Allocation int
}

func getConfig() Configuration {
	viper.SetEnvPrefix("KIAMOL")
	viper.AutomaticEnv()	
	viper.SetConfigFile("./config.toml")
	viper.ReadInConfig()
	config := Configuration{}	
    viper.Unmarshal(&config)    
	return config
}

// adapted from:
// https://golangcode.com/print-the-current-memory-usage/

func main() {

    config := getConfig()

    var overall [][]int
    //4.2M ints is about 1MB physical allocation:
    length := config.Allocation * 4200000
    count := 1

    for {
        // Allocate memory using make() and append to overall (so it doesn't get 
        // garbage collected). This is to create an ever increasing memory usage 
        // which we can track. We're just using []int as an example.
        a := make([]int, 0, length)
        overall = append(overall, a)

        fmt.Printf("Allocated ~%vMiB\n", count * config.Allocation)
        time.Sleep(time.Duration(config.Interval) * time.Second)
        count++
    }
}
