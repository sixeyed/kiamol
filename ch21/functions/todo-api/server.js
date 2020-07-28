const NATS = require('nats')
const nc = NATS.connect({url: 'nats://message-queue:4222', json: true})
const { v4: uuidv4 } = require('uuid');

function handler(event, context) {
  console.log('** todo-api handler called');
  
  var newItemEvent = {
    Subject: "events.todo.newitem",
    Item: {
      Item: event.data,
      DateAdded: new Date().toISOString()
    },
    CorrelationId: uuidv4()
  }

  nc.publish('events.todo.newitem', newItemEvent)
  console.log(`** New item published, event ID: ${newItemEvent.CorrelationId}`);
}

module.exports = { handler }