import json

def handler(event, context):
    new_item = json.loads(event.data)
    print(f"AUDIT @ {new_item['Item']['DateAdded']}: {new_item['Item']['Item']}")