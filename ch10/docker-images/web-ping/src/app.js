const https = require('https');
const log = require("./log");

const options = {
    hostname: process.env.TARGET,
    method: process.env.METHOD
  };

log.Logger.info('** web-ping ** Pinging: %s; method: %s; %dms intervals', options.hostname, options.method, process.env.INTERVAL);
  
let i = 1;
let start = new Date().getTime();
setInterval(() => {    
    start = new Date().getTime();
    log.Logger.debug('Making request number: %d; at %d', i++, start);
    var req = https.request(options, (res) => {
        var end = new Date().getTime();    
        var duration = end-start;    
        log.Logger.debug('Got response status: %s at %d; duration: %dms', res.statusCode, end, duration);
    });
    req.on('error', (e) => {
        console.error(e);
      });
    req.end();
}, process.env.INTERVAL)
