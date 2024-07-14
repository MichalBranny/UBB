function drawPyramid() {
    var vertices = new Float32Array([
        // Base
        -1, -1, -1,  1, -1, -1,  1, -1,  1, -1, -1,  1,
        // Sides
        -1, -1, -1,  1, -1, -1,  0,  1,  0,
         1, -1, -1,  1, -1,  1,  0,  1,  0,
         1, -1,  1, -1, -1,  1,  0,  1,  0,
        -1, -1,  1, -1, -1, -1,  0,  1,  0
    ]);

    var normals = new Float32Array([
        // Base
        0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0,
        // Sides
        0, 0.5, -0.5, 0, 0.5, -0.5, 0, 0.5, -0.5,
        0.5, 0.5, 0, 0.5, 0.5, 0, 0.5, 0.5, 0,
        0, 0.5, 0.5, 0, 0.5, 0.5, 0, 0.5, 0.5,
        -0.5, 0.5, 0, -0.5, 0.5, 0, -0.5, 0.5, 0
    ]);

    var indices = new Uint16Array([
        // Base
        0, 1, 2,  0, 2, 3,
        // Sides
        4, 5, 6,  7, 8, 9,  10, 11, 12,  13, 14, 15
    ]);

    gl.bindBuffer(gl.ARRAY_BUFFER, gl.createBuffer());
    gl.bufferData(gl.ARRAY_BUFFER, vertices, gl.STATIC_DRAW);
    gl.vertexAttribPointer(0, 3, gl.FLOAT, false, 0, 0);
    gl.enableVertexAttribArray(0);

    gl.bindBuffer(gl.ARRAY_BUFFER, gl.createBuffer());
    gl.bufferData(gl.ARRAY_BUFFER, normals, gl.STATIC_DRAW);
    gl.vertexAttribPointer(1, 3, gl.FLOAT, false, 0, 0);
    gl.enableVertexAttribArray(1);

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, gl.createBuffer());
    gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, indices, gl.STATIC_DRAW);

    gl.drawElements(gl.TRIANGLES, indices.length, gl.UNSIGNED_SHORT, 0);
}
