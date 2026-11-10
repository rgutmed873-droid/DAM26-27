#Actividad 18

# Actividad 18: Validar un DNI simplificado

#Hago función para validar el dni
def validar_dni_simple(dni):
    if len(dni) != 9:
        raise ValueError("El DNI debe tener 9 caracteres")
    

# 2. Programa principal
try:
    dni_usuario = input("Introduce tu DNI (8 números y 1 letra): ")
    
    validar_dni_simple(dni_usuario)
    
    
    # Si la función no lanzó ningún error, continuamos
    print(f"El DNI '{dni_usuario}' tiene una longitud válida.")

except ValueError as error:
    print(f"Error de validación: {error}")