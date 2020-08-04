const restify = require("restify");
const handler = require('./handler');

var server = restify.createServer();
server.use(restify.plugins.bodyParser());
server.post("/todos", handler.post);

server.listen(process.env.PORT, function() {
  console.log(`${server.name} listening at ${server.url}`);
});
