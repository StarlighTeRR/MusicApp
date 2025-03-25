import React from 'react';
import './Tree.css';

function TreeNode({ node, onNodeSelect }) {
  if (!node) return null;

  return (
    <li>
      <span onClick={() => onNodeSelect(node.id)}>{node.name}</span>
      {node.children && Array.isArray(node.children) && node.children.length > 0 && (
        <ul>
          {node.children.map((child) => (
            <TreeNode key={child.id} node={child} onNodeSelect={onNodeSelect} />
          ))}
        </ul>
      )}
    </li>
  );
}

function Tree({ data, onNodeSelect, filterText }) {
  const filterNodes = (nodes, text) => {
    return nodes.reduce((acc, node) => {
      const doesMatch = node.name.toLowerCase().includes(text.toLowerCase());
      let filteredChildren = [];
      if (node.children && Array.isArray(node.children)) {
        filteredChildren = filterNodes(node.children, text);
      }

      if (doesMatch || filteredChildren.length > 0) {
        acc.push({ ...node, children: filteredChildren });
      }

      return acc;
    }, []);
  };

  if (!data) {
    return <div>Нет данных для отображения</div>;
  }

  const dataArray = Array.isArray(data) ? data : [data];

  if (dataArray.length === 0) {
    return <div>Нет данных для отображения</div>;
  }

  const filteredData = filterText ? filterNodes(dataArray, filterText) : dataArray;

  return (
    <ul>
      {filteredData.map((node) => (
        <TreeNode key={node.id} node={node} onNodeSelect={onNodeSelect} />
      ))}
    </ul>
  );
}

export default Tree;
