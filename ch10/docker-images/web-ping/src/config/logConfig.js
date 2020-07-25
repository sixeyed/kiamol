const { format, transports } = require('winston');
var logConfig = module.exports = {};

logConfig.options = {
    transports: [
        new transports.Console({
            level: 'debug',
            format: format.combine(
                format.splat(),
                format.printf(log => {
                    return `${log.message}`
                })
            )
        })
    ]
};