const NATS = require('nats')
const nc = NATS.connect({url: 'nats://message-queue:4222', json: true})
const { v4: uuidv4 } = require('uuid');

function post(req, res, next) {
  console.log('** todo-api handler called');

  var newItemEvent = {
    Subject: "events.todo.newitem",
    Item: {
      Item: Object.keys(req.body)[0],
      DateAdded: new Date().toISOString()
    },
    CorrelationId: uuidv4()
  }

  nc.publish('events.todo.newitem', newItemEvent)
  console.log(`** New item published, event ID: ${newItemEvent.CorrelationId}`);

  res.send(201);
  next();
}

module.exports = { post }