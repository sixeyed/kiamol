const restify = require("restify");
const fs = require("fs");

const log = require("./log");
const validate = require('./validate');
const mutate = require('./mutate');

if (process.env.USE_HTTPS=="true") {
  var https_options = {
    key: fs.readFileSync('/run/secrets/tls/tls.key'),
    certificate: fs.readFileSync('/run/secrets/tls/tls.crt')
  };
  var server = restify.createServer(https_options);
}
else {
  var server = restify.createServer();
}
server.use(restify.plugins.bodyParser());

server.post("/validate", validate.post);
server.post("/mutate", mutate.post);

server.listen(process.env.PORT, function() {
  log.Logger.info("%s listening at %s", server.name, server.url);
});
