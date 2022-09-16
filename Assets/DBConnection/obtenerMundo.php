<?php
include_once "cors.php";
include_once "funciones.php";
$result = obtenerMundo();
echo json_encode($result);