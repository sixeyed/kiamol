<?php

function handler($event, $context) {
  fwrite(STDOUT, '* Mutating handler called.');
  $conn = pg_connect('host=todo-db dbname=todo user=postgres password=kiamol-2*2*');
  if (!$conn) {
    echo 'Connection failed';
    exit;
  }
  $sql = 'UPDATE "public"."ToDos" SET "Item"=\'Leave a nice review for KIAMOL :)\'';
  $result = pg_query($conn, $sql); 
  fwrite(STDOUT, '* Mutation complete.');
  return "* Mutated...";
}