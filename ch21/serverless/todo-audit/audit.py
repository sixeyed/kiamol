
def handler(event, context):    
    print(f"AUDIT @ {event['data']['Item']['DateAdded']}: {event['data']['Item']['Item']}")