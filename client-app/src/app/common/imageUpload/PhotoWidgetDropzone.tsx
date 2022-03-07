import React, { useCallback } from "react";
import { useDropzone } from "react-dropzone";
import { Header, Icon } from "semantic-ui-react";

interface Props {
	setFiles: (files: any) => void;
}

const PhotoWidgetDropzone = ({ setFiles }: Props) => {
	const styles = {
		border: "dashed 3px #eee",
		borderColor: "#eee",
		borderRadius: "5px",
		paddingTop: "30px",
		textAlign: "center" as 'center',
		height: 200,
	};
	const activeStyle = {
		borderColor: "green",
	};
	const onDrop = useCallback(
		(acceptedFiles) => {
			setFiles(
				acceptedFiles.map((file: any) =>
					Object.assign(file, { preview: URL.createObjectURL(file) }),
				),
			);
		},
		[setFiles],
	);
	const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop });

	return (
		<div
			{...getRootProps()}
			style={isDragActive ? { ...styles, ...activeStyle } : styles}
		>
			<input {...getInputProps()} />
            <Icon name="upload" size="huge" />
            <Header content='Drop image here'/>
		</div>
	);
};

export default PhotoWidgetDropzone;
