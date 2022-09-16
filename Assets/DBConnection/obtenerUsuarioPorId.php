<?php
include_once "cors.php";
$id = $_GET["id"];
include_once "funciones.php";
$usuario = obtenerUsuarioPorId($id);
echo json_encode($usuario);